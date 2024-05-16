using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using System.Net;

namespace StargateAPI.Business.Commands
{
    public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>
    {
        /// <summary>
        /// Name of the person.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Rank of the astronaut.
        /// </summary>
        public required string Rank { get; set; }

        /// <summary>
        /// Title of the duty.
        /// </summary>
        public required string DutyTitle { get; set; }

        /// <summary>
        /// Start date of the duty.
        /// </summary>
        public DateTime DutyStartDate { get; set; }

        /// <summary>
        /// end date of the duty.
        /// </summary>
        public DateTime DutyEndDate { get; set; }

        /// <summary>
        /// Indicates if this is the current duty.
        /// </summary>
        public bool IsCurrent { get; set; }
    }

    public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyPreProcessor(StargateContext context)
        {
            _context = context;
        }

        public Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);

            var verifyNoPreviousDuty = _context.AstronautDuties.FirstOrDefault(z => z.DutyTitle == request.DutyTitle && z.DutyStartDate == request.DutyStartDate);

            if (verifyNoPreviousDuty is not null) throw new BadHttpRequestException("Bad Request");

            return Task.CompletedTask;
        }
    }

    public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            try
            {
                var query = $"SELECT * FROM [Person] WHERE \'{request.Name}\' = Name";

                var person = await _context.Connection.QueryFirstOrDefaultAsync<Person>(query);

                if (person == null)
                {
                    throw new Exception("The referenced person does not exist in our system");
                }


                //if this is a current duty, ensure the person does not have any current duties
                if (request.IsCurrent)
                {
                    query = $"SELECT * FROM [AstronautDuty] WHERE \'{person.Id}\' = PersonId AND IsCurrent = 1";
                    var duty = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDuty>(query);

                    if (duty != null)
                    {
                        throw new Exception("A person can only have one current duty");
                    }
                }

                if (request.IsCurrent)
                {
                    // remove end date from current duties.
                    request.DutyEndDate = DateTime.MinValue;

                    var prevDuty = await _context.AstronautDuties
                                 .Where(d => d.PersonId == person.Id && d.IsCurrent)
                                 .OrderByDescending(d => d.DutyStartDate)
                                 .FirstOrDefaultAsync();

                    if (prevDuty != null)
                    {
                        // Set the end date of the previous duty to one day before the new duty's start date
                        prevDuty.DutyEndDate = request.DutyStartDate.AddDays(-1);
                        prevDuty.IsCurrent = false; // Mark the previous duty as not current
                        _context.AstronautDuties.Update(prevDuty);
                    }
                }




                query = $"SELECT * FROM [AstronautDetail] WHERE {person.Id} = PersonId";

                var astronautDetail = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDetail>(query);



                if (astronautDetail == null)
                {
                    astronautDetail = new AstronautDetail();
                    astronautDetail.PersonId = person.Id;
                    astronautDetail.CurrentDutyTitle = request.DutyTitle;
                    astronautDetail.CurrentRank = request.Rank;
                    astronautDetail.CareerStartDate = request.DutyStartDate.Date;
                    if (request.DutyTitle == "RETIRED")
                    {
                        //set retirement date to one day before the start of retirment duty
                        astronautDetail.CareerEndDate = request.DutyEndDate.Date.AddDays(-1); ;
                    }

                    await _context.AstronautDetails.AddAsync(astronautDetail);

                }
                else
                {
                    astronautDetail.CurrentDutyTitle = request.DutyTitle;
                    astronautDetail.CurrentRank = request.Rank;
                    if (request.DutyTitle == "RETIRED")
                    {
                        astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                    }
                    _context.AstronautDetails.Update(astronautDetail);
                }

                query = $"SELECT * FROM [AstronautDuty] WHERE {person.Id} = PersonId Order By DutyStartDate Desc";

                var astronautDuty = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDuty>(query);

                if (astronautDuty != null)
                {
                    astronautDuty.DutyEndDate = request.DutyStartDate.AddDays(-1).Date;
                    _context.AstronautDuties.Update(astronautDuty);
                }

                var newAstronautDuty = new AstronautDuty()
                {
                    PersonId = person.Id,
                    Rank = request.Rank,
                    DutyTitle = request.DutyTitle,
                    DutyStartDate = request.DutyStartDate.Date,
                    IsCurrent = request.IsCurrent,
                    DutyEndDate = null
                };

                await _context.AstronautDuties.AddAsync(newAstronautDuty);

                await _context.SaveChangesAsync();

                return new CreateAstronautDutyResult()
                {
                    Id = newAstronautDuty.Id
                };
            }
            catch (DbUpdateException ex)
            {
                return new CreateAstronautDutyResult()
                {
                    Success = false,
                    Message = ex.InnerException.Message
                };
            }


        }
    }

    public class CreateAstronautDutyResult : BaseResponse
    {
        public int? Id { get; set; }
    }
}

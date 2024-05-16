//unit test coverage proposal:

//insert new Person into Person Db : Jane Doe - assert that the record was succesfully created
//Attempt to insert Jane Doe into Person DB again - insert that this fails due to fk error

//insert new AtronautDuty for Jane Doe with isCurrent = false - assert that the record was succesfully created


//insert new AtronautDuty for Jane Doe with isCurrent = true. Assert the record was succesfully created.
//attempt to insert new  new AtronautDuty for Jane Doe with isCurrent = true - assert that this fails due pre-existing duty

//select the astronaugt duty for jane doe where iscurrent = true - assert that dutyEndDate = null

//insert new Person into Person Db : John Doe
//insert new AtronautDuty for John Doe with isCurrent = True, DutyTile = 'RETIRED' 
//Select AstronautDetails for John Doe - assert CareerEndDate is one day before dutystartdate for the retiredduty






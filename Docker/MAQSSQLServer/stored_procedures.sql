USE MagenicAutomation;
GO
CREATE PROCEDURE [dbo].[getStateAbbrevMatch]
       @StateAbbreviation VARCHAR(2)
  AS BEGIN
    SELECT StateAbbreviation FROM States
    WHERE StateAbbreviation = @StateAbbreviation
  END 
GO
CREATE PROCEDURE [dbo].[setStateAbbrevToSelf]
       @StateAbbreviation VARCHAR(2)
  AS BEGIN
    UPDATE States
    SET StateAbbreviation = @StateAbbreviation 
    WHERE StateAbbreviation = @StateAbbreviation
  END 
GO

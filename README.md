# LP-xlsfileupload


Custom section for excel file upload! 

On application start we create two database tables! The first one contains the data from the file, the second is only for audits!

The information from both of the tables is displayed in two separated tabs on the custom section!



Necessary actions:
1. update the structure of the custom db tables:

1.1. CompanyInventory:

Id; CompanyClient; CompanyName; IsOn; BC; Availability; CorporationStatus; DateOfIncorporation; Month; FirstOrSecondHalf; PotentialStrikeOffDate; ReservedDate; ReservedBy; SoldDate; SoldBy; CompanyStruckOffDate; Note; AuditId

1.2. CompanyInvetoryAudit

Id; ImportedDate; UserId; UserName; RecordsCount

2. Check the columns numbers in the SaveData function to correspond to your excel file structure!

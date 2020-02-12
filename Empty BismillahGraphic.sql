Use BismillahGraphic

DELETE FROM SellingPaymentRecord
DELETE FROM SellingList
DELETE FROM Selling

DELETE FROM Expanse
DELETE FROM Expanse_Category

DELETE FROM Product
DELETE FROM ProductCategory


DELETE FROM SMS_Recharge_Record
DELETE FROM SMS_Send_Record

DELETE FROM Vendor


DELETE FROM PageLinkAssign
DELETE FROM Registration WHERE (UserName <> N'Admin')

DELETE FROM AspNetUserRoles FROM AspNetUsers INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId WHERE (AspNetUsers.UserName <> N'Admin')

DELETE FROM AspNetUsers WHERE (UserName <> N'Admin')
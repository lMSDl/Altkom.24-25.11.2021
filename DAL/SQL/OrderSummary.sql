CREATE OR ALTER PROCEDURE OrderSummary
@id int
AS
BEGIN
	SELECT o.Id, o.[DateTime], SUM(p.Price) AS Price
	FROM [Order] AS o
	JOIN Product p ON p.OrderId = o.Id
	WHERE o.Id = @id
	GROUP BY o.id, o.[DateTime] 
END
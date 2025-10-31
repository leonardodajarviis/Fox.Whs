# ...existing code...
migration:
	dotnet ef migrations add $(name) --project Fox.Whs --startup-project Fox.Whs

migration-remove:
	dotnet ef migrations remove --project Fox.Whs --startup-project Fox.Whs

migration-update:
	dotnet ef database update --project Fox.Whs --startup-project Fox.Whs

publish:
	dotnet publish Fox.Whs/Fox.Whs.csproj -c Release -o Fox.Whs/publish
# ...existing code...
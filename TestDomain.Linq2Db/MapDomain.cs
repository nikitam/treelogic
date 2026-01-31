using LinqToDB.Mapping;

namespace TestDomain.Linq2Db;

public static class MapDomain	
{
	public static MappingSchema MappingSchema { get; private set; }
	
	public static void Map()
	{
		MappingSchema = new MappingSchema();
		var builder = new FluentMappingBuilder(MappingSchema);

		builder.Entity<ADDRESS>()
			.HasTableName("addresses")
			.HasSchemaName("public")
			.HasIdentity(x => x.Id)
			.HasPrimaryKey(x => x.Id)
			.Property(x => x.City).HasColumnName("city")
			.Association(x => x.User, x => x.UserId, x => x.Id);

		builder.Entity<USER>()
			.HasTableName("users")
			.HasSchemaName("public")
			.HasIdentity(x => x.Id)
			.HasPrimaryKey(x => x.Id)
			.Property(x => x.Id).HasColumnName("id")
			.Property(x => x.FirstName).HasColumnName("firstname")
			.Property(x => x.LastName).HasColumnName("lastname")
			.Property(x => x.Age).HasColumnName("age")
			.Association(x => x.Addresses, x => x.Id, x => x.Id);

		builder.Build();
	}
}
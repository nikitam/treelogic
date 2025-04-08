using LinqToDB.Mapping;

namespace TestDomain.Linq2Db;

public static class MapDomain	
{
	public static MappingSchema MappingSchema { get; private set; }
	
	public static void Map()
	{
		MappingSchema = new MappingSchema();
		var builder = new FluentMappingBuilder(MappingSchema);

		builder.Entity<USER>()
			.HasTableName("users")
			.HasSchemaName("public")
			.HasIdentity(x => x.Id)
			.HasPrimaryKey(x => x.Id)
			.Property(x => x.Id).HasColumnName("id")
			.Property(x => x.FirstName).HasColumnName("firstname")
			.Property(x => x.LastName).HasColumnName("lastname")
			.Property(x => x.Age).HasColumnName("age")
			//.Association(x => x.Vendor, x => x.VendorID, x => x.VendorID, canBeNull: false)
			;

//... other mapping configurations

// commit configured mappings to mapping schema
		builder.Build();
	}
}
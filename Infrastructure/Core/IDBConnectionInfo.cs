using System;

namespace Infrastructure.Core
{
	public interface IDBConnectionInfo
	{
		string GetSevername();

		string GetDatabasename();

		string GetUser();

		string GetPassword();

		string GetConnectionString();
	}
}


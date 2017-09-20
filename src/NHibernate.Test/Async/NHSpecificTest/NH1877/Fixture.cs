﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1877
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnSetUp()
		{
			using(var session=OpenSession())
			using(var tran=session.BeginTransaction())
			{
				session.Save(new Person {BirthDate = new DateTime(1988, 7, 21)});
				session.Save(new Person { BirthDate = new DateTime(1987, 7, 22) });
				session.Save(new Person { BirthDate = new DateTime(1986, 7, 23) });
				session.Save(new Person { BirthDate = new DateTime(1987, 7, 24) });
				session.Save(new Person { BirthDate = new DateTime(1988, 7, 25) });
				tran.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var tran = session.BeginTransaction())
			{
				session.CreateQuery("delete from Person").ExecuteUpdate();
				tran.Commit();
			}
		}

		[Test]
		public async Task CanGroupByWithPropertyNameAsync()
		{
			using(var session=OpenSession())
			{
				var crit = session.CreateCriteria(typeof (Person))
					.SetProjection(Projections.GroupProperty("BirthDate"),
					               Projections.Count("Id"));
				var result = await (crit.ListAsync());
				Assert.That(result,Has.Count.EqualTo(5));
			}
		}

		[Test]
		public async Task CanGroupByWithSqlFunctionProjectionAsync()
		{
			using (var session = OpenSession())
			{
				var crit = session.CreateCriteria(typeof (Person))
					.SetProjection(
					Projections.GroupProperty(Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("BirthDate"))));

				var result = await (crit.UniqueResultAsync());
				Assert.That(result,Is.EqualTo(7));
			}
		}
	}
}
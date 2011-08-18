﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using AspNet.Mvc.Tests.Helpers;
using ICSharpCode.AspNet.Mvc;
using NUnit.Framework;

namespace AspNet.Mvc.Tests
{
	[TestFixture]
	public class ModelClassLocatorTests
	{
		MvcModelClassLocator locator;
		FakeMvcProject fakeProject;
		FakeMvcParserService fakeParserService;
		
		void CreateLocator()
		{
			fakeProject = new FakeMvcProject();
			fakeParserService = new FakeMvcParserService();
			locator = new MvcModelClassLocator(fakeParserService);
		}
		
		List<IMvcClass> GetModelClasses()
		{
			return locator.GetModelClasses(fakeProject).ToList();
		}
		
		void AddModelClass(string fullyQualifiedClassName)
		{
			fakeParserService.AddModelClassToProjectContent(fullyQualifiedClassName);
		}
		
		string GetFirstModelClassName()
		{
			return GetModelClasses().First().FullyQualifiedName;
		}
		
		[Test]
		public void GetModelClasses_OneModelClassInProject_ReturnModelClassWithExpectedName()
		{
			CreateLocator();
			AddModelClass("MyNamespace.MyClass");
			string modelClassName = GetFirstModelClassName();
				
			Assert.AreEqual("MyNamespace.MyClass", modelClassName);
		}
		
		[Test]
		public void GetModelClasses_OneModelClassInProject_ReturnOneModelClass()
		{
			CreateLocator();
			AddModelClass("MyNamespace.MyClass");
			int count = GetModelClasses().Count;
				
			Assert.AreEqual(1, count);
		}
		
		[Test]
		public void GetModelClasses_NoModelClassesInProject_GetsProjectContentFromParserService()
		{
			CreateLocator();
			GetModelClasses();
			
			Assert.AreEqual(fakeProject, fakeParserService.ProjectPassedToGetProjectContent);
		}
	}
}
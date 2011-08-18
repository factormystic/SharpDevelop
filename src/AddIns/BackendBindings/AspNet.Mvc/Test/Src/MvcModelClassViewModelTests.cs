﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using AspNet.Mvc.Tests.Helpers;
using ICSharpCode.AspNet.Mvc;
using NUnit.Framework;

namespace AspNet.Mvc.Tests
{
	[TestFixture]
	public class MvcModelClassViewModelTests
	{
		FakeMvcClass fakeClass;
		MvcModelClassViewModel viewModel;
		
		void CreateViewModel(string classNamespace, string className)
		{
			fakeClass = new FakeMvcClass(classNamespace, className);
			viewModel = new MvcModelClassViewModel(fakeClass);
		}
		
		[Test]
		public void Name_ClassHasNamespaceAndName_ReturnsClassNameFollowedByNamespace()
		{
			CreateViewModel("ICSharpCode.Tests", "MyClass");
			string text = viewModel.Name;
			
			Assert.AreEqual("MyClass (ICSharpCode.Tests)", text);
		}
		
		[Test]
		public void ToString_ClassHasNamespaceAndName_ReturnsClassNameFollowedByNamespace()
		{
			CreateViewModel("ICSharpCode.Tests", "MyClass");
			string text = viewModel.ToString();
			
			Assert.AreEqual("MyClass (ICSharpCode.Tests)", text);
		}
	}
}
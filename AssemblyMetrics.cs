namespace ObjectOrientedMetrics
{
	using ObjectOrientedMetrics.Extensions;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class AssemblyMetrics
	{
		public readonly Assembly AssemblyInfo;
		public readonly List<ClassObject> Classes = new List<ClassObject>();

		public AssemblyMetrics(Assembly assembly) 
		{
			AssemblyInfo = assembly;

			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				if (!type.IsClass)
					continue;

				Classes.Add(new ClassObject(type, assembly));
			}
		}

		// Method Inheritance Factor
		public double? MIF()
		{
			var sumInheritedNotOverridedMethods = 0;
			var sumPublicMethods = 0;

			foreach (var classObj in Classes)
			{
				sumInheritedNotOverridedMethods += classObj.PublicMethods.Where(x => !x.IsOverride()).Count();
				sumPublicMethods += classObj.PublicMethods.Count();
			}

			if (sumPublicMethods == 0)
				return null;

			return sumInheritedNotOverridedMethods / (double)sumPublicMethods;
		}

		// Мethod Hiding Factor
		public double? MHF()
		{
			var sumPrivateMethods = 0;
			var sumAllMethods = 0;

			foreach (var classObj in Classes)
			{
				sumPrivateMethods += classObj.PrivateMethods.Count();
				sumAllMethods += classObj.AllMethodsCount;
			}

			if (sumAllMethods == 0)
				return null;

			return sumPrivateMethods / (double)sumAllMethods;
		}

		// Attribute Hiding Factor
		public double? AHF()
		{
			var sumPrivateAttributes = 0;
			var sumPublicDeclaredOnlyAttributes = 0;

			foreach (var classObj in Classes)
			{
				sumPrivateAttributes += classObj.PrivateAttributes.Count();
				sumPublicDeclaredOnlyAttributes += classObj.PublicDeclaredOnlyAttributes.Count();
			}

			var sumAllAttributesCount = sumPrivateAttributes + sumPublicDeclaredOnlyAttributes;
			if (sumAllAttributesCount == 0)
				return null;

			return sumPrivateAttributes / (double)sumAllAttributesCount;
		}

		// Attribute Inheritance Factor
		public double? AIF()
		{
			var sumPrivateAttributes = 0;
			var sumPublicAttributes = 0;
			var sumInheritedNotOverridedAttributes = 0;

			foreach (var classObj in Classes)
			{
				sumPrivateAttributes += classObj.PrivateAttributes.Count();
				sumPublicAttributes += classObj.PublicAttributes.Count();
				sumInheritedNotOverridedAttributes += classObj.InheritedNotOverridedAttributes.Count();
			}

			var sumAllAttributesCount = sumPrivateAttributes + sumPublicAttributes;
			if (sumAllAttributesCount == 0)
				return null;

			return sumInheritedNotOverridedAttributes / (double)sumAllAttributesCount;
		}

		// Polymorphism Object Factor
		public double? POF()
		{
			var sumProductNewMethodsOnChildren = 0;
			var sumInheritedOverridedMethods = 0;

			foreach (var classObj in Classes)
			{
				sumProductNewMethodsOnChildren += (classObj.PublicDeclaredOnlyMethods.Count() * classObj.Children.Count);
				sumInheritedOverridedMethods += classObj.PublicMethods.Where(x => x.IsOverride()).Count();
			}

			if (sumProductNewMethodsOnChildren == 0)
				return null;

			return sumInheritedOverridedMethods / (double)sumProductNewMethodsOnChildren;
		}

		public void PrintMetrics()
		{
			Console.WriteLine($"Assembly {AssemblyInfo.FullName}");
			Console.WriteLine($"------------------------------------------------");
			Console.WriteLine("MOOD metrics:");
			Console.WriteLine($"   MIF: {MetricToStr(MIF())}");
			Console.WriteLine($"   MHF: {MetricToStr(MHF())}");
			Console.WriteLine($"   AHF: {MetricToStr(AHF())}");
			Console.WriteLine($"   AIF: {MetricToStr(AIF())}");
			Console.WriteLine($"   POF: {MetricToStr(POF())}");
			Console.WriteLine($"------------------------------------------------");
		}

		private string MetricToStr(double? metric) 
		{ 
			return metric.HasValue ? $"{(int)(metric * 100)}%" : "NaN";
		}
	}
}

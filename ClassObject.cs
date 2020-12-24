namespace ObjectOrientedMetrics
{
	using ObjectOrientedMetrics.Extensions;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class ClassObject
	{
		public readonly Type ClassInfo;
		public readonly List<Type> InheritanceHierarchy;
		public readonly List<Type> Children;

		public readonly List<MethodInfo> PublicDeclaredOnlyMethods;
		public readonly List<MethodInfo> PublicMethods;
		public readonly List<MethodInfo> PrivateMethods;

		public readonly List<FieldInfo> PublicDeclaredOnlyAttributes;
		public readonly List<FieldInfo> PublicAttributes;
		public readonly List<FieldInfo> PrivateAttributes;
		public readonly List<FieldInfo> InheritedNotOverridedAttributes;

		public readonly int AllMethodsCount;

		public ClassObject(Type type, Assembly assembly)
		{
			if (!type.IsClass)
				throw new ArgumentException($"Invalid type {type}");

			ClassInfo = type;
			InheritanceHierarchy = type.GetInheritanceHierarchy().ToList();
			Children = type.GetAllSubclassOf(assembly).ToList();

			var declaredOnlyFlag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
			var publicFlag = BindingFlags.Public | BindingFlags.Instance;
			var privateFlag = BindingFlags.NonPublic | BindingFlags.Instance;

			PublicDeclaredOnlyMethods = type.GetMethods(declaredOnlyFlag).ToList();
			PublicMethods = type.GetMethods(publicFlag).ToList();
			PrivateMethods = type.GetMethods(privateFlag).ToList();
			AllMethodsCount = PublicMethods.Count() + PrivateMethods.Count();

			PublicDeclaredOnlyAttributes = type.GetFields(declaredOnlyFlag).ToList();
			PublicAttributes = type.GetFields(publicFlag).ToList();
			PrivateAttributes = type.GetFields(privateFlag).ToList();

			var attributeNames = PublicDeclaredOnlyAttributes.Select(x => x.Name).ToList();
			InheritedNotOverridedAttributes = PublicAttributes
				.Where(x => !attributeNames.Contains(x.Name))
				.GroupBy(x => x.Name)
				.Select(g => g.First())
				.ToList();
		}

		public double? MIF()
		{
			if (AllMethodsCount == 0)
				return null;

			var inheritedNotOverridedMethodsCount = PublicMethods.Where(x => !x.IsOverride()).Count();

			return inheritedNotOverridedMethodsCount / (double)AllMethodsCount;
		}

		public double? MHF()
		{
			if (AllMethodsCount == 0)
				return null;

			return PrivateMethods.Count() / (double)AllMethodsCount;
		}

		public double? AHF()
		{
			var allAttributesCount = PrivateAttributes.Count() + PublicDeclaredOnlyAttributes.Count();
			if (allAttributesCount == 0)
				return null;

			return PrivateAttributes.Count() / (double)allAttributesCount;
		}

		public double? AIF()
		{
			var allAttributesCount = PrivateAttributes.Count() + PublicAttributes.Count();
			if (allAttributesCount == 0)
				return null;

			return InheritedNotOverridedAttributes.Count() / (double)allAttributesCount;
		}

		public double? POF()
		{
			var productNewMethodsOnChildren = PublicDeclaredOnlyMethods.Count() * Children.Count();
			if (productNewMethodsOnChildren == 0)
				return null;

			var inheritedOverridedMethods = PublicMethods.Where(x => x.IsOverride()).ToList();

			return inheritedOverridedMethods.Count() / (double)productNewMethodsOnChildren;
		}

		public void PrintMetrics()
		{
			Console.WriteLine($"Class {ClassInfo.Name}");
			Console.WriteLine($"------------------------------------------------");
			Console.WriteLine($"Depth of Inheritance Tree: {InheritanceHierarchy.Count()} [{ClassInfo.Name}{InheritanceHierarchy.ExtendedToString(" => ")}] ");
			Console.WriteLine($"Number of children: {Children.Count()} [{Children.ExtendedToString(", ")}] ");
			Console.WriteLine($"------------------------------------------------");
		}
	}
}
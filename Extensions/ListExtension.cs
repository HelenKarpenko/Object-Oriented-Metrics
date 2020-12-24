namespace ObjectOrientedMetrics.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public static class ListExtension
	{
		public static string ExtendedToString(this List<Type> list, string divider)
		{
			var last = list.LastOrDefault();
			StringBuilder sb = new StringBuilder();
			foreach (var item in list)
			{
				sb.Append($"{item.Name}");
				if (item != last)
					sb.Append(divider);
			}

			return sb.ToString();
		}
	}
}

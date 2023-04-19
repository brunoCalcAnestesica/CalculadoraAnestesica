using System;
using Xamarin.Forms;

namespace CalculadoraAnestesica.Extensions
{
	public static class ViewExtensions
	{
		public static void AddToStack(this
			Xamarin.Forms.View view, StackLayout stackLayout)
		{
			stackLayout.Children.Add(view);
		}
	}
}


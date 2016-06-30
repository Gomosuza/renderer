namespace RenderingTargetSample
{
	public class Program
	{
		public static void Main()
		{
			using (var game = new Game())
			{
				game.Run();
			}
		}
	}
}

namespace ObjectConstructors
{
    public static class GenerateSceneExtensions
    {
        public static IConstructible[] GetObjects()
        {
            return new[] { new FieldConstructor() };
        }
    }
}
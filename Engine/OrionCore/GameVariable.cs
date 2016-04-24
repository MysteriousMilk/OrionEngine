namespace Orion.Core
{
    public enum GameVariableType
    {
        Int,
        Float,
        String
    }

    public class GameVariable
    {
        public string Name { get; set; }
        public GameVariableType Type { get; set; }
        public object Value { get; set; }

        public GameVariable()
        {

        }

        public GameVariable(string name, GameVariableType type, object value)
        {

        }
    }
}

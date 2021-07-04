using System;

namespace GoFigure.App.Model
{
    class CharacterAttribute : Attribute
    {
        public char Symbol { get; private set; }

        public CharacterAttribute(char symbol) => 
            Symbol = symbol;
    }
}

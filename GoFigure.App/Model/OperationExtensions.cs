using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GoFigure.App.Model
{
    public static class OperationExtensions
    {
        private static readonly IDictionary<char, Operator> CharacterToOperator =
            typeof(Operator).GetMembers(BindingFlags.Public | BindingFlags.Static)
                .Select(m => 
                    (
                        member: m,
                        charAttributes: m.GetCustomAttributes(
                            typeof(CharacterAttribute),
                            true
                        )
                    )
                )
                .Where(t => t.charAttributes.Length > 0)
                .ToDictionary(
                    t => (t.charAttributes.FirstOrDefault() as CharacterAttribute).Symbol,
                    t => (Operator)Enum.Parse(typeof(Operator), t.member.Name)
                );

        private static readonly IDictionary<Operator, char> OperatorToCharacter =
            CharacterToOperator.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public static char ToCharacter(this Operator op) =>
            OperatorToCharacter[op];

        public static bool IsOperator(this char character) =>
            CharacterToOperator.ContainsKey(character);

        public static Operator ToOperator(this char character) =>
            CharacterToOperator[character];
    }
}

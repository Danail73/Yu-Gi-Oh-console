﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessLayer;


namespace PresentationLayer
{
    public class DisplayCardMenu
    {
        private string prompt = @"
  ______                             __        ______             ______          
 /      \                           /  |      /      |           /      \         
/$$$$$$  |  ______    ______    ____$$ |      $$$$$$/  _______  /$$$$$$  |______  
$$ |  $$/  /      \  /      \  /    $$ |        $$ |  /       \ $$ |_ $$//      \ 
$$ |       $$$$$$  |/$$$$$$  |/$$$$$$$ |        $$ |  $$$$$$$  |$$   |  /$$$$$$  |
$$ |   __  /    $$ |$$ |  $$/ $$ |  $$ |        $$ |  $$ |  $$ |$$$$/   $$ |  $$ |
$$ \__/  |/$$$$$$$ |$$ |      $$ \__$$ |       _$$ |_ $$ |  $$ |$$ |    $$ \__$$ |
$$    $$/ $$    $$ |$$ |      $$    $$ |      / $$   |$$ |  $$ |$$ |    $$    $$/ 
 $$$$$$/   $$$$$$$/ $$/        $$$$$$$/       $$$$$$/ $$/   $$/ $$/      $$$$$$/  
              
";
        public Card Card { get; set; }
        public string[] Options { get; set; }
        private string cardText;
        public Menu cardMenu;
        public int SelectedIndex { get; set; }

        public DisplayCardMenu(Card card)
        {
            Card = card;
            Options = GetOptions();
            cardMenu = new Menu(prompt, Options);
        }
        

        public int RunMenu()
        {
            SelectedIndex = cardMenu.Run();
            return SelectedIndex;
        }

        private string[] GetOptions()
        {
            cardText = GetCatdText() ;
            string cardType = Card.CardType;
            if (cardType.Equals("monster"))
            {
                return MonsterOptions();
            }
            else if (cardType.Equals("spell"))
            {
                return SpellOptions();
            }
            return TrapOptions();
        }

        private string[] MonsterOptions()
        {
            return [
                $"Id: {Card.CardId}",
                $"Name: {Card.Name}",
                $"Type: {Card.CardType}",
                $"Attribute: {Card.Attribute}",
                $"Level: {Card.Level}",
                $"ATK: {Card.ATK}",
                $"DEF: {Card.DEF}",
                $"Card text: {cardText}",
                $"View card",
                $"Back"
            ];
        }
        private string[] SpellOptions()
        {
            return [
                $"Id: {Card.CardId}",
                $"Name: {Card.Name}",
                $"Spell Type: {Card.Property}",
                $"Card text: {cardText}",
                $"View card",
                $"Back"
            ];
        }
        private string[] TrapOptions()
        {
            return [
                $"Id: {Card.CardId}",
                $"Name: {Card.Name}",
                $"Trap Type: {Card.Property}",
                $"Card text: {cardText}",
                $"View card",
                $"Back"
            ];
        }
        private string GetCatdText()
        {
            string cardEffect = Card.EffectText;
            
            int index = 0;
            double window = Console.WindowWidth - 14;
            StringBuilder sb = new StringBuilder();
            for(int i=0; i<(int)Math.Floor(cardEffect.Length/window);i++)
            {
                string line = cardEffect.Substring(index, (int)window);
                if(Regex.IsMatch(line, @"\r\n?|\n"))
                {
                    if(i == 0)
                    {
                        Match match = Regex.Match(line, @"\r\n?|\n");
                        int end = match.Index;
                        line = new string(cardEffect.Substring(index, Math.Abs(end - index)));
                    }
                    else
                    {
                        Regex.Replace(line, @"\r\n?|\n", " ");
                    }
                }
                index += line.Length;
                sb.Append(line.Trim());
                sb.Append("\n              ");
            }
            if (index < cardEffect.Length - 1)
            {
                if (cardEffect.Length - 1 - index > window)
                {
                    string preLine = cardEffect.Substring(index, (int)window);
                    sb.Append(preLine.Trim());
                    sb.Append("\n              ");
                    index += preLine.Length;
                }
                sb.Append(cardEffect.Substring(index, cardEffect.Length - 1 - index).Trim());
            }
            return sb.ToString();
        }
    }
}

﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Translations;
using CommandVote.Configs;

namespace CommandVote
{
    public class CommandVote : BasePlugin, IPluginConfig<CommandVoteConfig>
    {
        public override string ModuleName => "CommandVote";
        public override string ModuleVersion => "1.0.0";
        public override string ModuleAuthor => "Kewaii";
        public override string ModuleDescription => "Add votation to execute a specific command";

        Dictionary<string, int> Votes = new();
        public CommandVoteConfig Config { get; set; } = new CommandVoteConfig();
        public void OnConfigParsed(CommandVoteConfig config)
        {
            this.Config = config;
        }

        public override void Load(bool hotReload)
        {
            foreach(var aliasToCommand in Config.AliasToCommandSet)
            {
                string alias = aliasToCommand[0];
                string desc = aliasToCommand[2];
                string firstArg = alias.Split(' ')[0];
                this.AddCommand(firstArg.StartsWith($"css_") ? firstArg : $"css_{firstArg}", desc, (player, info) =>
                {
                    string command = alias.Replace(firstArg, "").Trim();
                    string commandRun = info.ArgString;                
                    if (commandRun.Equals(command))
                    {                        
                        if (player != null && player.IsValid)
                        {
                            if (Votes.TryGetValue(alias, out var voted))
                            {
                                
                                Votes[alias] += 1;
                                Execute(aliasToCommand, player);
                            } else {
                                Votes[alias] = 1;
                                Execute(aliasToCommand, player);                                
                            }
                    }
                    }
                    Console.WriteLine(commandRun);
                    Console.WriteLine(command);                    
                });
            }
        }
        public void Execute(string[] aliasToCommand, CBasePlayerController player)
        {
            if (Votes[aliasToCommand[0]] >= Int32.Parse(aliasToCommand[3]))
            {
                Server.ExecuteCommand(aliasToCommand[1]);
                foreach (CCSPlayerController playerOnline in Utilities.GetPlayers())
                {
                    if (playerOnline != null && playerOnline.IsValid)
                    {
                        playerOnline.PrintToChat($"{Localizer["VotedSuccess"]} {aliasToCommand[2]}");
                    }
                }
            }
            else 
            {
                foreach (CCSPlayerController playerOnline in Utilities.GetPlayers())
                {
                    if (playerOnline != null && playerOnline.IsValid)
                    {
                        playerOnline.PrintToChat($"{player.PlayerName} {Localizer["VotedIn"]} {aliasToCommand[2]}");
                        playerOnline.PrintToChat($"{Localizer["TypePrefix"]} \"{aliasToCommand[0].Replace("css_", "!")}\" {Localizer["TypeSuffix"]}");
                }
                }
            }
            
        }
    }
}

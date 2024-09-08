using CounterStrikeSharp.API.Core;
using System;
using System.Text;
using System.Text.Json.Serialization;

namespace CommandVote.Configs
{
    public class CommandVoteConfig : BasePluginConfig
    {
        [JsonPropertyName("alias_to_command_list")]
        public string[][] AliasToCommandSet { get; set; } = [["css_kewaii nao morras", "css_respawn kewaii", "Revive o kewaii!"]];
    }
}
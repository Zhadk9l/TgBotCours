using System;
using System.Collections.Generic;
using System.Text;

namespace TgBotCours
{
    internal class HeroDotaResponse
    {

        public int id { get; set; }
        public string name { get; set; }
        public string localized_name { get; set; }
        public string attack_type { get; set; }
        public List<string> roles { get; set; }
        public float base_health { get; set; }
        public float base_health_regen { get; set; }
        public float base_mana { get; set; }
        public float base_mana_regen { get; set; }
        public float base_armor { get; set; }
        public float base_attack_min { get; set; }
        public float base_attack_max { get; set; }
        public float attack_range { get; set; }
        public float move_speed { get; set; }

    }
}

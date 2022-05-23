namespace TgBotCours
{
    public class LiveMatches
    {
        public long activate_time { get; set; }
        public long league_id { get; set; }
        public int game_time { get; set; }
        public int average_mmr { get; set; }
        public long match_id { get; set; }
        public string team_name_radiant { get; set; }
        public string team_name_dire { get; set; }
        public int team_id_radiant { get; set; }
        public int team_id_dire { get; set; }
        public int radiant_score { get; set; }
        public int dire_score { get; set; }


    }
}

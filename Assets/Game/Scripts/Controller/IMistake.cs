namespace Game.Controllers
{
    public interface IMistake 
    {
        float HealthBorder { get; set; }

        void CharacterHasMadeMistake();
    }
}
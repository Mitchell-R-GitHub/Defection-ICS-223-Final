public class GameStartUI : UIItem
{
    public void StartGame()
    {
        Messenger.Broadcast(GameEvent.GAME_START);
    }
}

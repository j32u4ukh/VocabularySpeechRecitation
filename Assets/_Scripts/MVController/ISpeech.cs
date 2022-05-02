namespace VTS
{
    public interface ISpeech
    {
        //Receive status message from callback
        public void onStatusListener(string message);

        //Callback handler for start speaking
        public void onStartListener();

        //Callback handler for finish speaking
        public void onDoneListener();

        //Callback handler for interrupt speaking
        public void onStopListener(string message);
    }
}

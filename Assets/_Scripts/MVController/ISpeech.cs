namespace vts
{
    public interface ISpeech
    {
        //Receive status message from callback
        public void onStatus(string message);

        //Callback handler for start speaking
        public void onStart();

        //Callback handler for finish speaking
        public void onDone();

        //Callback handler for interrupt speaking
        public void onStop(string message);
    }
}

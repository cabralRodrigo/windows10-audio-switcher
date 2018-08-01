namespace AudioSwitcher.Util
{
    public static class Extensions
    {
        public static int? ToRegistryValue(this StateType state)
        {
            switch (state)
            {
                case StateType.Mono:
                    return 1;
                case StateType.Stereo:
                    return 0;
                case StateType.Unknown:
                default:
                    return null;
            }
        }
    }
}
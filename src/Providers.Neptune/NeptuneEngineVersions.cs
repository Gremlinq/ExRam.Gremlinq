namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// The released versions of the Amazon Neptune engine that
    /// <see cref="NeptuneConfiguratorExtensions.WithEngineVersion(INeptuneConfigurator, Version)"/> knows the TinkerPop versions of,
    /// as listed in AWS's <see href="https://docs.aws.amazon.com/neptune/latest/userguide/engine-releases.html">engine releases</see>,
    /// from 1.3.2.0, the first engine on TinkerPop 3.7.
    /// </summary>
    public static class NeptuneEngineVersions
    {
        /// <summary>Neptune engine 1.3.2.0.</summary>
        public static readonly Version V1_3_2_0 = new(1, 3, 2, 0);

        /// <summary>Neptune engine 1.3.2.1.</summary>
        public static readonly Version V1_3_2_1 = new(1, 3, 2, 1);

        /// <summary>Neptune engine 1.3.3.0.</summary>
        public static readonly Version V1_3_3_0 = new(1, 3, 3, 0);

        /// <summary>Neptune engine 1.3.4.0.</summary>
        public static readonly Version V1_3_4_0 = new(1, 3, 4, 0);

        /// <summary>Neptune engine 1.4.0.0.</summary>
        public static readonly Version V1_4_0_0 = new(1, 4, 0, 0);

        /// <summary>Neptune engine 1.4.1.0.</summary>
        public static readonly Version V1_4_1_0 = new(1, 4, 1, 0);

        /// <summary>Neptune engine 1.4.2.0.</summary>
        public static readonly Version V1_4_2_0 = new(1, 4, 2, 0);

        /// <summary>Neptune engine 1.4.3.0.</summary>
        public static readonly Version V1_4_3_0 = new(1, 4, 3, 0);

        /// <summary>Neptune engine 1.4.4.0.</summary>
        public static readonly Version V1_4_4_0 = new(1, 4, 4, 0);

        /// <summary>Neptune engine 1.4.5.0.</summary>
        public static readonly Version V1_4_5_0 = new(1, 4, 5, 0);

        /// <summary>Neptune engine 1.4.5.1.</summary>
        public static readonly Version V1_4_5_1 = new(1, 4, 5, 1);

        /// <summary>Neptune engine 1.4.6.0.</summary>
        public static readonly Version V1_4_6_0 = new(1, 4, 6, 0);

        /// <summary>Neptune engine 1.4.6.1.</summary>
        public static readonly Version V1_4_6_1 = new(1, 4, 6, 1);

        /// <summary>Neptune engine 1.4.6.2.</summary>
        public static readonly Version V1_4_6_2 = new(1, 4, 6, 2);

        /// <summary>Neptune engine 1.4.6.3.</summary>
        public static readonly Version V1_4_6_3 = new(1, 4, 6, 3);

        /// <summary>Neptune engine 1.4.7.0.</summary>
        public static readonly Version V1_4_7_0 = new(1, 4, 7, 0);

        /// <summary>Neptune engine 1.4.7.1.</summary>
        public static readonly Version V1_4_7_1 = new(1, 4, 7, 1);

        /// <summary>Neptune engine 1.4.8.0.</summary>
        public static readonly Version V1_4_8_0 = new(1, 4, 8, 0);

        /// <summary>Neptune engine 1.4.8.1.</summary>
        public static readonly Version V1_4_8_1 = new(1, 4, 8, 1);

        /// <summary>Neptune engine 1.5.0.0.</summary>
        public static readonly Version V1_5_0_0 = new(1, 5, 0, 0);
    }
}

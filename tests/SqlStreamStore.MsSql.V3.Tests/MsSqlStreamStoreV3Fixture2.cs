namespace SqlStreamStore
{
    using System.Threading.Tasks;
    using SqlStreamStore.Infrastructure;

    public sealed class MsSqlStreamStoreV3Fixture2 : IStreamStoreFixture
    {
        private readonly MsSqlStreamStoreV3Fixture _fixture;

        private MsSqlStreamStoreV3Fixture2(
            MsSqlStreamStoreV3Fixture fixture,
            MsSqlStreamStoreV3 store,
            GetUtcNow getUtcNow)
        {
            _fixture = fixture;
            Store = store;
            GetUtcNow = getUtcNow;
        }

        IStreamStore IStreamStoreFixture.Store => Store;

        public MsSqlStreamStoreV3 Store { get; }

        public GetUtcNow GetUtcNow
        {
            get => _fixture.GetUtcNow;
            set => _fixture.GetUtcNow = value;
        }

        public long MinPosition { get; set; } = 0;

        public int MaxSubscriptionCount { get; set; } = 500;

        public static async Task<MsSqlStreamStoreV3Fixture2> Create(
            string schema = "dbo",
            bool deleteDatabaseOnDispose = true,
            GetUtcNow getUtcNow = null,
            string databaseName = null)
        {
            getUtcNow = getUtcNow ?? SystemClock.GetUtcNow;
            var fixture = new MsSqlStreamStoreV3Fixture(schema, deleteDatabaseOnDispose, databaseName)
            {
                GetUtcNow = getUtcNow
            };
            var streamStore = await fixture.GetMsSqlStreamStore();
            return new MsSqlStreamStoreV3Fixture2(fixture, streamStore, getUtcNow);
        }

        public static async Task<MsSqlStreamStoreV3Fixture2> CreateUninitialized(
            string schema = "dbo",
            bool deleteDatabaseOnDispose = true,
            GetUtcNow getUtcNow = null,
            string databaseName = null)
        {
            getUtcNow = getUtcNow ?? SystemClock.GetUtcNow;
            var fixture = new MsSqlStreamStoreV3Fixture(schema, deleteDatabaseOnDispose, databaseName)
            {
                GetUtcNow = getUtcNow
            };
            var streamStore = await fixture.GetUninitializedStreamStore();
            return new MsSqlStreamStoreV3Fixture2(fixture, streamStore, getUtcNow);
        }

        public void Dispose()
        {
            Store.Dispose();
            _fixture.Dispose();
        }
    }
}
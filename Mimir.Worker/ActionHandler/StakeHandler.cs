using Bencodex.Types;
using Libplanet.Crypto;
using Mimir.Worker.CollectionUpdaters;
using Mimir.Worker.Services;
using MongoDB.Driver;
using Nekoyume.Action;
using Serilog;

namespace Mimir.Worker.ActionHandler;

public class StakeHandler(IStateService stateService, MongoDbService store) :
    BaseActionHandler(stateService, store, "^stake[0-9]*$", Log.ForContext<StakeHandler>())
{
    private static readonly Stake Action = new();

    protected override async Task<bool> TryHandleAction(
        long blockIndex,
        Address signer,
        IValue actionPlainValue,
        string actionType,
        IValue? actionPlainValueInternal,
        IClientSessionHandle? session = null,
        CancellationToken stoppingToken = default)
    {
        Action.LoadPlainValue(actionPlainValue);
        await StakeCollectionUpdater.UpdateAsync(
            StateService,
            Store,
            signer,
            session,
            stoppingToken);
        return true;
    }
}

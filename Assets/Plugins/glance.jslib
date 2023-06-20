var plugin = {
    RewardedAd: function(type)
    {  let type1 = UTF8ToString(type);
         _triggerReason = type1;
        rewardEvent();
    },
    ReplayAd: function(type)
    {  let type1 = UTF8ToString(type);
        _triggerReason = type1;
        replayEvent();
    },
    LoadAnalytics: function()
    {  
        sendCustomAnalyticsEvent("game_load", {} );
    },
    StartAnalytics: function()
    {  
        sendCustomAnalyticsEvent("game_start", {});
    },
    ReplayAnalytics: function(level)
    {  
        sendCustomAnalyticsEvent("game_replay", {level: level});
    },
    EndAnalytics: function(level)
    {  
    sendCustomAnalyticsEvent("game_end", {level: level});
    },
    LevelAnalytics: function(level)
    {  
    sendCustomAnalyticsEvent("game_level", {level: level});
    },
    LevelCompletedAnalytics: function(level)
    {  
    sendCustomAnalyticsEvent("game_levelcompleted", {level: level});
    },
    RewardedAdsAnalytics: function(successCBf,failureCBf)
    {   let successCBf1 = UTF8ToString(successCBf);
        let failureCBf1 = UTF8ToString(failureCBf);
    sendCustomAnalyticsEvent("game_rewarded", {successCB: successCBf1, failureCB: failureCBf1});
    },
    MilestoneAnalytics: function(CollectedStars,level)
    {  
    sendCustomAnalyticsEvent("game_milestone", {type: 'Stars', meta: {StarCount: CollectedStars}, level: level, score: 0, highScore: 0});
    },
    GameLifeEndAnalytics: function(RemainingLife,level)
    {  
    sendCustomAnalyticsEvent("game_lifeend", {level: level,score: 0,highScore: 0,remainingLife: RemainingLife});
    },
    IngameAnalytics: function(items,amount,level)
    {
        let items1 = UTF8ToString(items);
        sendCustomAnalyticsEvent("game_ingame_transaction", { transactionType: items1, meta: {item_bought: items1,coin_spent: amount}, level: level, score: 0, highScore: 0});    
    }
};

mergeInto(LibraryManager.library, plugin);
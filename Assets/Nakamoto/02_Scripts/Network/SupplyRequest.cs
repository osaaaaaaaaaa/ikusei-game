//---------------------------------------------------------------
//
// 調達用リクエスト [ SupplyRequest.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/29
// Update:2024/10/29
//
//---------------------------------------------------------------
using Newtonsoft.Json;

public class SupplyRequest
{
    /// 調達後の数
    [JsonProperty("food_vol")]
    public int FoodVol { get; set; }
}

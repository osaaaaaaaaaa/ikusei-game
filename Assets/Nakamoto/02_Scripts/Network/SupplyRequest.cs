//---------------------------------------------------------------
//
// ���B�p���N�G�X�g [ SupplyRequest.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/29
// Update:2024/10/29
//
//---------------------------------------------------------------
using Newtonsoft.Json;

public class SupplyRequest
{
    /// ���B��̐�
    [JsonProperty("food_vol")]
    public int FoodVol { get; set; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    /// <summary>  
    /// プレイヤー  
    /// </summary>  
    [SerializeField] private Player player_ = null;

    /// <summary>  
    /// ワールド行列   
    /// </summary>  
    private Matrix4x4 worldMatrix_ = Matrix4x4.identity;

    /// <summary>  
    /// ターゲットとして設定する  
    /// </summary>  
    /// <param name="enable">true:設定する / false:解除する</param>  
    public void SetTarget(bool enable)
    {
        // マテリアルの色を変更する  
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.materials[0].color = enable ? Color.red : Color.white;
    }

	/// <summary>
	/// 開始処理
	/// </summary>
	public void Start()
    {
		worldMatrix_.SetTRS(transform.position,Quaternion.identity, Vector3.one);
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    public void Update()
    {
		var normalZ = new Vector3(0, 0, 1);
		var EnemyForward =worldMatrix_ * normalZ;		
		var EnemyToPlayer = (player_.transform.position - transform.position).normalized;
		var EnemyViewCos = Mathf.Cos(20.0f * Mathf.Deg2Rad);
		var dot = Vector3.Dot(EnemyForward, EnemyToPlayer);
		if (EnemyViewCos <= dot)
		{
			var cross = Vector3.Cross(EnemyForward, EnemyToPlayer);
			var radian = Mathf.Min(Mathf.Acos(dot),(10.0f * Mathf.Deg2Rad));
			radian *= (cross.y / Mathf.Abs(cross.y));
			var rotMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, Mathf.Rad2Deg * radian, 0));
			worldMatrix_ = worldMatrix_ * rotMatrix;

			var f = new Vector3(0, 0, 0.2f);
			var move = worldMatrix_ * f;

			var pos = worldMatrix_.GetColumn(3) + move;
			worldMatrix_.SetColumn(3, pos);
		}
		transform.position = worldMatrix_.GetColumn(3);
		transform.rotation = worldMatrix_.rotation;
		transform.localScale = worldMatrix_.lossyScale;
	}
}

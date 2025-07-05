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
		Quaternion rot = Quaternion.identity;
		worldMatrix_.SetTRS(player_.transform.position - transform.position, rot, Vector3.one);
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    public void Update()
    {
		var normalZ = new Vector3(0, 0, 1);
		var EnemyForward = player_.worldMatrix * normalZ;
		var EnemyViewCos = Mathf.Cos(player_.viewRadian);
		Matrix4x4 scaleMatrix = Matrix4x4.identity;
		Matrix4x4 transMatrix = Matrix4x4.identity;
		Vector4 trans = new Vector4();
		Vector3 scale = new Vector3();
		var EnemyToPlayer = (player_.transform.position - transform.position).normalized;
		var dot = Vector3.Dot(EnemyForward, EnemyToPlayer);
		if (EnemyViewCos <= dot)
		{
			Debug.Log("A");
			trans.Set(0f, 0f, 0f, 0);
			scale.Set(1f, 1f, 1f);
			trans.z = 0.2f * Time.deltaTime;
			var toTarget = (player_.transform.position - transform.position).normalized;
			var foward = transform.forward;
			if (0.999f < dot) { return; }
			var radian = Mathf.Acos(dot);
			var cross = Vector3.Cross(foward,toTarget);
			radian *= (cross.y / Mathf.Abs(cross.y));
			transform.rotation *= Quaternion.Euler(0, Mathf.Rad2Deg * radian, 0);
			scaleMatrix = scaleMatrix * Matrix4x4.Scale(scale);
			transMatrix = transMatrix * Matrix4x4.Scale(trans);
			Matrix4x4 matrix = (transMatrix * scaleMatrix) * worldMatrix_;
			transform.position = matrix.GetColumn(3);
		}

	}
}

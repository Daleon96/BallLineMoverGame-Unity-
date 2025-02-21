using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : UIWindow
{
    [SerializeField] private List<BlockSkinDate> _blockSkinDateList;
    [SerializeField] private BlockSkinDate defaultSkin;
    [SerializeField] private TextMeshProUGUI titleTMP;
    [SerializeField] private Image headImage;
    [SerializeField] private Image tailImage;
    [SerializeField] private Image previsionButton;
    [SerializeField] private Image nextButton;
    [SerializeField] private GameObject coinArea;
    [SerializeField] private TextMeshProUGUI costTMP;
    [SerializeField] private TextMeshProUGUI actionButtonTMP;

    private int currentShopSkinId = 0;
    
    public override void OpenWindow()
    {
        base.OpenWindow();
        currentShopSkinId = PrefsController.Instance.GetCurrentBlockSkin();

        if (currentShopSkinId == 0 && _blockSkinDateList.Count > 0)
        {
            PrefsController.Instance.SetCurrentBlockSkin(defaultSkin.id);
            currentShopSkinId = defaultSkin.id;
        }
        ReloadUI();
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
    }

    public void Next()
    {
        BlockSkinDate blockSkinDate = GetCurrentBlockSkin();
        if (blockSkinDate != null)
        {
            int index = _blockSkinDateList.FindIndex(it => it == blockSkinDate) + 1;
            index = Mathf.Clamp(index, 0, _blockSkinDateList.Count() - 1);
            currentShopSkinId = _blockSkinDateList[index].id;
            ReloadUI();
        }
    }
    public void Prevision()
    {
        BlockSkinDate blockSkinDate = GetCurrentBlockSkin();
        if (blockSkinDate != null)
        {
            int index = _blockSkinDateList.FindIndex(it => it == blockSkinDate) - 1;
            index = Mathf.Clamp(index, 0, _blockSkinDateList.Count() - 1);
            currentShopSkinId = _blockSkinDateList[index].id;
            ReloadUI();
        }
    }

    public void Action()
    {
        BlockSkinDate blockSkinDate = GetCurrentBlockSkin();
        if (PrefsController.Instance.IsSkinBuyed(blockSkinDate.id) || blockSkinDate.cost == 0)
        {
            PrefsController.Instance.SetCurrentBlockSkin(blockSkinDate.id);
        }
        else
        {
            if (PrefsController.Instance.GetCoinCount() >= blockSkinDate.cost)
            {
                PrefsController.Instance.SetCoinCount(
                    PrefsController.Instance.GetCoinCount() - blockSkinDate.cost);
                PrefsController.Instance.BuySkin(blockSkinDate.id);
            }
        }
        ReloadUI();
    }


    public void ReloadUI()
    {
        BlockSkinDate blockSkinDate = GetCurrentBlockSkin();
        if (blockSkinDate != null)
        {
            titleTMP.text = blockSkinDate.title;
            headImage.sprite = blockSkinDate.mainSprite;
            tailImage.sprite = blockSkinDate.tailSprite;
            costTMP.text = "" + blockSkinDate.cost;
            if (_blockSkinDateList.Count > 0)
            {
                previsionButton.gameObject.SetActive(blockSkinDate != _blockSkinDateList[0]);
                nextButton.gameObject.SetActive(blockSkinDate != _blockSkinDateList[_blockSkinDateList.Count() - 1]);
            }

            if (PrefsController.Instance.IsSkinBuyed(blockSkinDate.id) || blockSkinDate.cost == 0)
            {
                if (PrefsController.Instance.GetCurrentBlockSkin() == blockSkinDate.id)
                {
                    coinArea.gameObject.SetActive(false);
                    actionButtonTMP.gameObject.SetActive(true);
                    actionButtonTMP.text = "Current";
                }
                else
                {
                    coinArea.gameObject.SetActive(false);
                    actionButtonTMP.gameObject.SetActive(true);
                    actionButtonTMP.text = "Choose";
                }
            }
            else
            {
                actionButtonTMP.gameObject.SetActive(false);
                coinArea.gameObject.SetActive(true);
            }
            
        }
    }

    public BlockSkinDate GetCurrentBlockSkin()
    {
        BlockSkinDate blockSkin = _blockSkinDateList.Find(it => it.id == currentShopSkinId);
    
        return blockSkin; 
    }
}

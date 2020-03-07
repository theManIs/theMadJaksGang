using System;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Proxies;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses
{
    public class CursorChanger : IUserAwake
    {
        private Texture2D _cursorTexture;
        private Texture2D _baseCursor;
        private Texture2D _pickCursor;
        private Texture2D _cursorShoot;
        private Texture2D _moveCursor;
        private Texture2D _outRange;
        private Texture2D _outOfRangeRanged;
        private Texture2D _allyCursor;

        public void UserAwake(AwakeParams ap)
        {
            throw new System.NotImplementedException();
        }

        public void UserStart(StartParams sp)
        {
            _cursorShoot = Resources.Load<Texture2D>("G_Cursor_shoot_1530036");
            _cursorTexture = Resources.Load<Texture2D>("Cursor_Attack_-42768");
            _baseCursor = Resources.Load<Texture2D>("G_Cursor_Basic2_1530026");
            _pickCursor = Resources.Load<Texture2D>("G_Cursor_Hand_1530032");
            _moveCursor = Resources.Load<Texture2D>("G_Cursor_Move1_1530142");
            _outRange = Resources.Load<Texture2D>("G_Cursor_Attack_R");
            _outOfRangeRanged = Resources.Load<Texture2D>("Arrow_R_72020");
            _allyCursor = Resources.Load<Texture2D>("G_Cursor_Settings_132668");
        }

        public void SetCursor(Transform softFocus, Transform hardFocus)
        {
            MonoMechanicus monomech = softFocus ? softFocus.GetComponent<MonoMechanicus>() : null;
            MonoMechanicus monomechHard = hardFocus ? hardFocus.GetComponent<MonoMechanicus>() : null;
            MonoAmplifierRpg monoRpg = hardFocus ? hardFocus.GetComponent<MonoAmplifierRpg>() : null;
            bool isRanged = monoRpg && monoRpg.WieldingWeapon && monoRpg.WieldingWeapon.isRanged;
            float attackRange = monoRpg && monoRpg.WieldingWeapon ? monoRpg.WieldingWeapon.WeaponRange : default;
            bool isWithinRange = softFocus && hardFocus && attackRange > Vector3.Distance(softFocus.position, hardFocus.position);

            if (monomechHard && monomech && monomechHard.IsBlueTeam == monomech.IsBlueTeam)
                SetAnyCursor(_allyCursor);
            else if (softFocus && hardFocus && hardFocus != softFocus && monomech && isRanged && !isWithinRange)
                SetAnyCursor(_outOfRangeRanged);
            else if (softFocus && hardFocus && hardFocus != softFocus && monomech && isRanged && isWithinRange)
                SetShootCursor();
            else if (softFocus && hardFocus && hardFocus != softFocus && monomech && !isRanged && isWithinRange)
                SetAggressiveCursor();
            else if (softFocus && hardFocus && hardFocus != softFocus && monomech && !isRanged && !isWithinRange)
                SetAnyCursor(_outRange);
            else if (softFocus && hardFocus && softFocus == hardFocus)
                SetChooseCursor();
            else if (hardFocus)
                SetMoveCursor();
            else
                SetDefaultCursor();
        }

        public void SetCursorIfOnUi(Transform softFocus, Transform hardFocus, StatesList states)
        {
            if (states.UserOnUi && !states.LockModeOn)
                SetDefaultCursor();
            else if (states.LockModeOn)
                SetAggressiveCursor();
            else
                SetCursor(softFocus, hardFocus);
        }

        private void SetShootCursor() => SetAnyCursor(_cursorShoot);
        private void SetMoveCursor() => SetAnyCursor(_moveCursor);
        public void SetChooseCursor() => SetAnyCursor(_pickCursor);
        public void SetDefaultCursor() => Cursor.SetCursor(_baseCursor, new Vector2(75,20), CursorMode.Auto);
        public void SetAggressiveCursor() => Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);

        private void SetAnyCursor(Texture2D cursorTexture) =>
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
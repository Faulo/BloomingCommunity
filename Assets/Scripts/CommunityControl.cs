using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class CommunityControl {
        readonly CommunityAsset asset;
        readonly MapControl map;
        ECommunityState state;

        public CommunityControl(CommunityAsset asset, MapControl map) {
            this.asset = asset;
            this.map = map;
        }

        float stateTimer;

        public void FixedUpdate(float deltaTime) {
            switch (state) {
                case ECommunityState.None:
                    stateTimer -= deltaTime;
                    if (stateTimer <= 0) {
                        state = ECommunityState.Travellers;

                        SpawnTravellers();

                        if (stateTimer < 0) {
                            FixedUpdate(Mathf.Abs(stateTimer));
                        }

                        return;
                    }

                    break;
                case ECommunityState.Travellers:
                    stateTimer = Random.Range(asset.minWaitForTravellers, asset.maxWaitForTravellers);
                    state = ECommunityState.None;

                    break;
            }
        }

        void SpawnTravellers() {
            Debug.Log("spawned travllers");
        }
    }
}
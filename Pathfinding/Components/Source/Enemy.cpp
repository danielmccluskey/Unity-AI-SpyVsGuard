#include "AudioComponent.h"
#include "BulletComponent.h"
#include "Enemy.h"
#include "Entity.h"

#include "TransformComponent.h"

typedef Entity PARENT;

Enemy::Enemy(PEER_TYPE a_PEERTYPE)
{
	//Add new components to the component list
	AddComponent(new TransformComponent(this));
	AddComponent(new AudioComponent(this));
	AddComponent(new BulletComponent(this));
}

Enemy::~Enemy()
{
}

void Enemy::Update(float a_fDeltaTime)
{
	PARENT::Update(a_fDeltaTime);
}

void Enemy::Draw(unsigned int a_uiProgramID, unsigned int a_uiVBO, unsigned int a_uiIBO)
{
	PARENT::Draw(a_uiProgramID, a_uiVBO, a_uiIBO);
}

void Enemy::AddSound(bool a_bStartInstantly, FMOD::Studio::EventDescription *& a_EventName, int a_iSoundType)
{
	AudioComponent* pAudioComp = static_cast<AudioComponent*>(this->FindComponentOfType(COMPONENT_TYPE::AUDIO));
	if (pAudioComp)//Null check
	{
		pAudioComp->AddSound(a_EventName, a_bStartInstantly, a_iSoundType);
	}
}
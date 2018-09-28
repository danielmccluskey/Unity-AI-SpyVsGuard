#include "AudioComponent.h"
#include "BulletComponent.h"
#include "Entity.h"

#include "TransformComponent.h"
#include <iostream>
typedef Component PARENT;

BulletComponent::BulletComponent(Entity * pOwner)
	: PARENT(pOwner)
{
	m_eComponentType = COMPONENT_TYPE::BULLET;

	for (int i = 0; i < m_iMaxBullets; i++)//Initialise all the bullets
	{
		BulletArray TempBullet;
		m_vBulletArray.push_back(TempBullet);
	}
}

void BulletComponent::Update(float a_fDeltaTime)
{
	for (int i = 0; i < m_vBulletArray.size(); i++)//Update the bullet array
	{
		if (m_vBulletArray[i].m_bActive == true)
		{
			m_vBulletArray[i].Update(a_fDeltaTime);
		}
		if (m_vBulletArray[i].m_bActive == false)
		{
		}
	}
}
void BulletComponent::Shoot()
{
	if (GetOwnerEntity())//Null check
	{
		TransformComponent* pLocalTransformComp = static_cast<TransformComponent*>(GetOwnerEntity()->FindComponentOfType(COMPONENT_TYPE::TRANSFORM));
		if (pLocalTransformComp)//Null check
		{
			AudioComponent* pAudioComp = static_cast<AudioComponent*>(GetOwnerEntity()->FindComponentOfType(COMPONENT_TYPE::AUDIO));

			if (pAudioComp)//Null check
			{
				for (int i = 0; i < m_vBulletArray.size(); i++)//Loop through the bullet array and find the first available bullet
				{
					if (m_vBulletArray[i].m_bActive == false)//Null check
					{
						m_vBulletArray[i].Shoot(pLocalTransformComp->GetCurrentPosition(), pLocalTransformComp->GetFacingDirection());//Shoot
						pAudioComp->StartSound(AudioType::AUDIO_ATTACK);//Play shoot sound
					}
				}
			}
		}
	}
}

void BulletArray::Shoot(glm::vec3 a_v3Pos, glm::vec3 a_v3Velocity)
{
	//Magic numbers, best numbers.
	m_bActive = true;
	m_v3BulletDirection = a_v3Velocity;
	m_v3BulletDirection.y = 0;
	m_v3BulletPosition = a_v3Pos;
	m_v3BulletPosition.y += 2;

	m_fLifeTime = 2.0f;
	m_fSpeed = 300.0f;
}

void BulletArray::Update(float a_fDelta)
{
	m_v3BulletPosition += (m_v3BulletDirection * m_fSpeed) * a_fDelta;//Move the bullet in its direction

	m_fLifeTime -= a_fDelta;//Decrease Lifetime

	if (m_fLifeTime <= 0)//If the bullet should delete
	{
		m_bActive = false;//Delete bullet
	}
}
// ***********************************************************************
// Assembly         :
// Author           : Daniel McCluskey - s1600056
// Created          : 05-22-2018
//
// Last Modified By : Daniel McCluskey - s1600056
// Last Modified On : 05-22-2018
// ***********************************************************************
#ifndef _BULLET_COMPONENT_H_
#define _BULLET_COMPONENT_H_

//includes
#include "Component.h"
#include <vector>

class Entity;

class BulletArray
{
public:
	BulletArray()
	{
		m_bActive = false;
	}
	bool m_bActive = false;//Whether or not to update the bullet or not
	float m_fLifeTime;//How long the bullet is alive for
	float m_fSpeed;//The speed of the bullet
	glm::vec3 m_v3BulletDirection;//The Directional vector of the bullet
	glm::vec3 m_v3BulletPosition;//The current position of the bullet

	//Function to activate a bullet and fire it from a position in a certain direction
	//glm::vec3 a_v3Pos = The position to fire it from
	//glm::vec3 a_v3Velocity = The direction to fire it in
	void Shoot(glm::vec3 a_v3Pos, glm::vec3 a_v3Velocity);

	//Function to update the bullet
	//float a_fDeltaTime = Time since last frame
	void Update(float a_fDelta);
};

class BulletComponent : public Component
{
public:

	~BulletComponent();
	BulletComponent(Entity* pOwner);//Contructor
	int m_iMaxBullets = 30;//Max amount of bullets allowed
	std::vector<BulletArray> m_vBulletArray;//Array of bullets

	//Function to update all of the bullets in m_vBulletArray
	//float a_fDeltaTime = Time since last frame
	virtual void Update(float a_fDeltaTime);

	//Get the owner entity to fire a bullet
	void Shoot();
};

#endif // !_BULLET_COMPONENT_H_

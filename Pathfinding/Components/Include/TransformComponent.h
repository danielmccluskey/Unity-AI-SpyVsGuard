// ***********************************************************************
// Assembly         :
// Author           : Daniel McCluskey - s1600056
// Created          : 05-22-2018
//
// Last Modified By : Daniel McCluskey - s1600056
// Last Modified On : 05-22-2018
// ***********************************************************************

#ifndef _TRANSFORM_COMPONENT_H_
#define _TRANSFORM_COMPONENT_H_

//includes
#include "Component.h"

class Entity;

enum MATRIX_ROW
{
	RIGHT_VECTOR,
	UP_VECTOR,
	FORWARD_VECTOR,
	POSITION_VECTOR,
};

class TransformComponent : public Component
{
public:
	/// <summary>
	/// Initializes a new instance of the <see cref="TransformComponent"/> class.
	/// </summary>
	/// <param name="a_pOwner">The owner entity.</param>
	TransformComponent(Entity* a_pOwner);
	/// <summary>
	/// Finalizes an instance of the <see cref="TransformComponent"/> class.
	/// </summary>
	~TransformComponent();

	/// <summary>
	/// Gets the right direction.
	/// </summary>
	/// <returns>glm.vec3.</returns>
	glm::vec3 GetRightDirection();
	/// <summary>
	/// Gets up direction.
	/// </summary>
	/// <returns>glm.vec3.</returns>
	glm::vec3 GetUpDirection();
	/// <summary>
	/// Gets the facing direction.
	/// </summary>
	/// <returns>glm.vec3.</returns>
	glm::vec3 GetFacingDirection();
	/// <summary>
	/// Gets the current position.
	/// </summary>
	/// <returns>glm.vec3.</returns>
	glm::vec3 GetCurrentPosition();

	/// <summary>
	/// Sets the right direction.
	/// </summary>
	/// <param name="vRightDirection">The v right direction.</param>
	void SetRightDirection(glm::vec3 vRightDirection);
	/// <summary>
	/// Sets up direction.
	/// </summary>
	/// <param name="vUpDirection">The v up direction.</param>
	void SetUpDirection(glm::vec3 vUpDirection);
	/// <summary>
	/// Sets the facing direction.
	/// </summary>
	/// <param name="vFacingDirection">The v facing direction.</param>
	void SetFacingDirection(glm::vec3 vFacingDirection);
	/// <summary>
	/// Sets the current position.
	/// </summary>
	/// <param name="vCurrentPosition">The v current position.</param>
	void SetCurrentPosition(glm::vec3 vCurrentPosition);

	/// <summary>
	/// Gets the entity matrix.
	/// </summary>
	/// <returns>const glm.mat4 &.</returns>
	const glm::mat4& GetEntityMatrix();

	const void SetEntityMatrix(glm::mat4 a_Matrix);

private:
	/// <summary>
	/// Sets the entity matrix row.
	/// </summary>
	/// <param name="eRow">The e row.</param>
	/// <param name="vVec">The v vec.</param>
	void SetEntityMatrixRow(MATRIX_ROW eRow, glm::vec3 vVec);
	/// <summary>
	/// Gets the entity matrix row.
	/// </summary>
	/// <param name="eRow">The e row.</param>
	/// <returns>glm.vec3.</returns>
	glm::vec3 GetEntityMatrixRow(MATRIX_ROW eRow);

	/// <summary>
	/// The m m4 entity matrix
	/// </summary>
	glm::mat4 m_m4EntityMatrix;
};

#endif
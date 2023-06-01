//
// Copyright (c) 2017-2023 the rbfx project.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

#pragma once
#include "../Math/Vector2.h"
#include "../Math/Vector3.h"
#include "../Math/Vector4.h"
#include "../Math/Quaternion.h"
#include "../Math/Color.h"
#include "../Math/Rect.h"
#include "../Math/Matrix2.h"
#include "../Math/Matrix3.h"
#include "../Math/Matrix3x4.h"
#include "../Math/Matrix4.h"

#include <duktape/duktape.h>

namespace Urho3D
{
    URHO3D_API Vector2 rbfx_vector2_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API Vector3 rbfx_vector3_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API IntVector2 rbfx_int_vector2_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API IntVector3 rbfx_int_vector3_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API Vector4 rbfx_vector4_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API Quaternion rbfx_quaternion_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API Color rbfx_color_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API Rect rbfx_rect_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API IntRect rbfx_int_rect_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API Matrix2 rbfx_matrix2_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API Matrix3 rbfx_matrix3_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API Matrix3x4 rbfx_matrix3x4_resolve(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API Matrix4 rbfx_matrix4_resolve(duk_context* ctx, duk_idx_t stack_idx);
}

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
#include <duktape/duktape.h>

namespace Urho3D
{
    /// @{
    /// add setTimeout, setInterval, clearTimeout, clearInterval
    /// and rbfx.clearAllTimers methods to JS environment.
    /// @}
    void js_setup_timer_bindings(duk_context* ctx);
    /// @{
    /// execute scheduled timers
    /// @}
    void js_resolve_timers(duk_context* ctx);
    /// @{
    /// clear scheduled timers. this method doesn't remove
    /// pointers from global stash.
    /// to remove then, call js_clear_internal_timers instead
    /// @}
    void js_clear_all_timers();
    /// @{
    /// clear stored timers on global stash.
    /// this method doesn't free scheduled timers
    /// to do this, call js_clear_all_timers instead
    /// @}
    void js_clear_internal_timers(duk_context* ctx);
}

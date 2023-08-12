#
# Copyright (c) 2017-2023 the rbfx project.
#
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:
#
# The above copyright notice and this permission notice shall be included in
# all copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
# THE SOFTWARE.
#

project (JSBindTool VERSION 0.1.0 LANGUAGES CSharp)

file(TO_NATIVE_PATH "${CMAKE_CURRENT_SOURCE_DIR}" JS_BIND_TOOL_SRC_DIR)

get_filename_component(JS_BINDINGS_SRC_DIR "${CMAKE_CURRENT_SOURCE_DIR}/../../JSBindings" ABSOLUTE)

if (URHO3D_JS_MOBILE)
    set (JS_BIND_TOOL_DEFINES "URHO3D_MOBILE")
else ()
    set (JS_BIND_TOOL_DEFINES "URHO3D_DESKTOP")
endif ()

if (URHO3D_ACTIONS)
    list(APPEND JS_BIND_TOOL_DEFINES "URHO3D_ACTIONS")
endif ()

set (JS_BIND_TOOL_WORK_DIR "${CMAKE_BINARY_DIR}/${DEST_BIN_DIR${_CONFIG}}") 
file (TO_NATIVE_PATH "${JS_BIND_TOOL_WORK_DIR}" JS_BIND_TOOL_WORK_DIR)

# normalize windows path like to JSON
string (REPLACE "\\" "\\\\" JS_BIND_TOOL_WORK_DIR ${JS_BIND_TOOL_WORK_DIR})
string (REPLACE "\\" "\\\\" JS_BINDINGS_SRC_DIR ${JS_BINDINGS_SRC_DIR})

configure_file("${PROJECT_NAME}.csproj.template" "${PROJECT_NAME}.csproj")
configure_file("launchSettings.json.template" "Properties/launchSettings.json")
include_external_msproject(${PROJECT_NAME} ${CMAKE_CURRENT_BINARY_DIR}/${PROJECT_NAME}.csproj)
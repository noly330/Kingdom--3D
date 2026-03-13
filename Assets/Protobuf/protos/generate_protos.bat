set curr_cmd_path=%cd%

set protoc_path=.\Assets\Protobuf\bin
set proto_path=.\Assets\Protobuf\protos
set csharp_gen_path=.\Assets\Protobuf\output

del /q "%csharp_gen_path%\*"

"%protoc_path%\protoc.exe" --csharp_out=%csharp_gen_path% %proto_path%\*.proto
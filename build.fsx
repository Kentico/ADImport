#r @"packages/FAKE.Core/tools/FakeLib.dll"
open Fake

// Properties
let outputDir = "./Bin/"
let slnFile = "./ADImport.sln"

// Targets
Target "Clean" (fun _ ->
    CleanDir outputDir
)

Target "Compile" (fun _ ->
    !!slnFile
    |> MSBuildRelease "" "Rebuild"
    |> ignore
)

Target "Default" DoNothing

// Dependencies
"Clean"
  ==> "Compile"
  ==> "Default"

RunTargetOrDefault "Default"

# Renderer

An old 3D mesh-based, opinionated, monogame renderer.

Can render 2D surfaces (rendered as quads right infront of the camera) as well as 3D meshes and natively supports rendertargets..

All meshes are rendered with brushes and/or pens, allowing for both fill and outlines.

Filling supports different materials such as SolidColorBrush, TextureBrush, VertexColorBrush, etc. and can be easily extended.

## IRenderContext

Core interface that allows both 2D and 3D mesh rendering. Implemented via the DefaultRenderContext.

## IMeshCreator

Instance available on the IRenderContext. Allows the creation of Mesh and DynamicMesh via meshdescriptions. A mesh description from a model is not provided but can easily be added.

Supports dynamic and static mesh creation and the GenericMeshDescriptionBuilder has basic shape methods (plane, box, room) available.

## Mesh

Static mesh that can simply be drawn with a pen and brush.

## DynamicMesh

Dynamic mesh allows to swap parts or the entire mesh.
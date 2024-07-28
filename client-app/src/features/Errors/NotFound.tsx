import { Link } from "react-router-dom";
import { Button, Header, Icon, Segment } from "semantic-ui-react";

export default function NotFound() {
  return (
    <Segment placeholder>
      <Header icon>
        <Icon name="search">
          Oops - we've looked every where but counld not fine what you are
          looking for
        </Icon>
      </Header>
      <Segment.Inline>
        <Button as={Link} to="/activities">
          Return To Activities Page
        </Button>
      </Segment.Inline>
    </Segment>
  );
}
